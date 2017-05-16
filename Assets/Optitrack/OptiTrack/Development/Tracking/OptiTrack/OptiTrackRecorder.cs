using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(OptiTrackManager))]
public class OptiTrackRecorder : MonoBehaviour {

    bool recording = false;
    bool streaming = false;
    RecordingData data;

    public Texture2D image_LED;

    private string recordName = "motiveRecord";

    private string CurrentDataLog = "";

    public bool gui = true;

    float recordStart;

    OptiTrackManager _optitrackManager;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Record());
        StartCoroutine(Stream());
    }
	
	// Update is called once per frame
	void Update () {
        if (_optitrackManager == null)
            _optitrackManager = GetComponent<OptiTrackManager>();
    }

    private void OnGUI()
    {
        if (!gui)
            return;

        GUI.Label(new Rect(5, 5, 200, 25), "MiReVi Motive Recorder");
        GUI.Label(new Rect(5, 35, 50, 25), "Name: ");
        recordName = GUI.TextField(new Rect(55, 35, 150, 25), recordName);

        if (!recording)
            GUI.color = new Color(0.3f, 0.05f, 0.05f);
        else
            GUI.color = new Color(0.9f, 0.1f, 0.1f);

        GUI.DrawTexture(new Rect(210, 65, 25, 25), image_LED);

        if (streaming)
            GUI.color = new Color(0.1f, 0.9f, 0.1f);
        else
            GUI.color = new Color(0.05f, 0.3f, 0.05f);

        GUI.DrawTexture(new Rect(210, 185, 25, 25), image_LED);

        GUI.color = Color.white;
        if (GUI.Button(new Rect(5, 65, 200, 25), "Start Record"))
        {
            StartRecord();
        }
        if (GUI.Button(new Rect(5, 95, 200, 25), "Stop Record"))
        {
            StopRecord();
        }

        if (GUI.Button(new Rect(5, 125, 200, 25), "Save Data"))
        {
            SaveData(recordName);
        }
        if (GUI.Button(new Rect(5, 155, 200, 25), "Load Data"))
        {
            LoadData(recordName);
        }

        if (GUI.Button(new Rect(5, 185, 200, 25), "Start Stream"))
        {
            StartStream();
        }
        if (GUI.Button(new Rect(5, 215, 200, 25), "Stop Stream"))
        {
            StopStream();
        }

        GUI.color = Color.green;

        GUI.Label(new Rect(260, 5, 600, 900),CurrentDataLog);

    }

    void LoadData(string suffix)
    {
        string path = "MotiveRecorder";
        string name = "";

        CurrentDataLog = "Try loading " + suffix + ".mrc.";

        if (!File.Exists(path + "/" + name + suffix + ".mrc"))
        {

            CurrentDataLog += "\nCan't find File.";
            return;
        }

        try
        {
            FileStream recordStream = File.OpenRead(path + "/" + name + suffix + ".mrc");

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            data = (RecordingData)binaryFormatter.Deserialize(recordStream);

            recordStream.Close();

            CurrentDataLog += "\nloading " + suffix + ".mrc done.";
            Debug.Log("loaded " + name + suffix + ".mrc. " + data.GetFrameCount() + " frames.");
        }
        catch(System.Exception)
        {
            CurrentDataLog += "\nloading " + suffix + ".mrc failed!";
        }
    }

    void SaveData(string suffix)
    {
        string path = "MotiveRecorder";
        string name = "";
        CurrentDataLog = "Saving " + suffix + ".mrc.";

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("create directory");
            CurrentDataLog += "\nmissing directory -> creating";
        }

        try
        {
            FileStream recordStream = File.Create(path + "/" + name + suffix + ".mrc");

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            binaryFormatter.Serialize(recordStream, data);

            recordStream.Close();

            CurrentDataLog += "\n" + suffix + ".mrc saved.";

        }
        catch(System.Exception)
        {
            CurrentDataLog += "\nCan't save" + suffix + ".mrc !";
        }
    }

    void StartRecord()
    {
        data = new RecordingData();
        recording = true;
        streaming = false;

        recordStart = Time.time;
    }

    void StopRecord()
    {
        recording = false;
    }

    void StartStream()
    {
        recording = false;
        streaming = true;
            _optitrackManager.currentSource = OptiTrackManager.Sources.record;
    }

    void StopStream()
    {
        streaming = false;
        if (_optitrackManager != null)
            _optitrackManager.currentSource = OptiTrackManager.Sources.live;
    }

    IEnumerator Record()
    {

        yield return null;

        while (true)
        {
            if (recording)
            {
                TimeFrame newFrame = new TimeFrame();

                //get Motive Markers and Rigidbodies, convert to Lists, save
                
                List<Vector3> ump = GetComponent<OptiTrackManager>().GetAllUnlabeledMarkerList();

                for(int i = 0;i<ump.Count;i++)
                {
                    newFrame.AddMarker(ump[i]);
                }

                List<OptitrackManagement.OptiTrackRigidBody> tbl = _optitrackManager.GetAllRigidBodyList();

                for (int i = 0; i < tbl.Count; i++)
                {
                    newFrame.AddRigidbody(tbl[i].position, tbl[i].orientation, tbl[i].ID);
                }                

                data.AddFrame(newFrame);

                CreateCurrentDataLog("Recording", newFrame);
            }
            yield return null;
        }
    }

    IEnumerator Stream()
    {

        yield return null;

        int t = 0;

        while (true)
        {
            if (streaming && data!=null)
            {
                //stream marker and rigidbody data
                if (t < data.GetFrameCount()-1)
                    t++;
                else
                    t = 0;

                TimeFrame currentFrame = data.GetFrame(t);
                CreateCurrentDataLog("Streaming",currentFrame);
                
                if(_optitrackManager != null)
                {
                    _optitrackManager.ClearUnlabeledMarkerPositions();

                    foreach (MotiveRigidbody rb in currentFrame.motiveRigidbodies)
                        _optitrackManager.ReceiveRigidbodyData(1,new Vector3(rb.posX,rb.posY,rb.posZ),new Quaternion(rb.rotX,rb.rotY,rb.rotZ,rb.rotW),rb.id);

                    foreach (MotiveMarker um in currentFrame.motiveMarkers)
                        _optitrackManager.ReceiveUnlabeledMarkerData(1,new Vector3(um.posX,um.posY,um.posZ));
                }

            }
            else
            {
                t = 0;
            }
            yield return null;
        }
    }

    void CreateCurrentDataLog(string action, TimeFrame currentFrame)
    {
        if (!gui)
            return;

        CurrentDataLog = action + "\t" + recordName + ".mrc\n";
        CurrentDataLog += "Frame: " + currentFrame.count + "\n\n";

        CurrentDataLog += "markers: " + currentFrame.motiveMarkers.Count;
        foreach (MotiveMarker mm in currentFrame.motiveMarkers)
        {
            CurrentDataLog += "\n" + string.Format("{0:0.###}", mm.posX) + "\t" + string.Format("{0:0.###}", mm.posY) + "\t" + string.Format("{0:0.###}", mm.posZ);
        }

        CurrentDataLog += "\n\nrigidbodies: " + currentFrame.motiveRigidbodies.Count;
        foreach (MotiveRigidbody mrb in currentFrame.motiveRigidbodies)
        {
            CurrentDataLog += "\n" + mrb.id + " p:\t" + string.Format("{0:0.###}", mrb.posX) + "\t" + string.Format("{0:0.###}", mrb.posY) + "\t" + string.Format("{0:0.###}", mrb.posZ);
            Vector3 rot = new Quaternion(mrb.rotX, mrb.rotY, mrb.rotZ, mrb.rotW).eulerAngles;
            CurrentDataLog += "\tr:\t" + string.Format("{0:0}", rot.x) + "\t" + string.Format("{0:0}", rot.y) + "\t" + string.Format("{0:0}", rot.z);
        }
    }
}

[System.Serializable]
public class RecordingData
{
    int count;
    List<TimeFrame> frames = new List<TimeFrame>();

    public void AddFrame(TimeFrame frame)
    {
        frame.count = frames.Count;
        frames.Add(frame);

        count++;
    }

    public TimeFrame GetFrame(int t)
    {
        return frames[t];
    }

    public int GetFrameCount()
    {
        return count;
    }

}

[System.Serializable]
public class TimeFrame
{
    public int count;
    public List<MotiveMarker> motiveMarkers = new List<MotiveMarker>();
    public List<MotiveRigidbody> motiveRigidbodies = new List<MotiveRigidbody>();

    public void AddMarker(Vector3 position)
    {
        motiveMarkers.Add(new MotiveMarker(position));
    }

    public void AddRigidbody(Vector3 position, Quaternion rotation, int id)
    {
        motiveRigidbodies.Add(new MotiveRigidbody(position, rotation, id));
    }
}

[System.Serializable]
public class MotiveMarker
{
    public float posX;
    public float posY;
    public float posZ;

    public MotiveMarker(Vector3 pos)
    {
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
    }
}

[System.Serializable]
public class MotiveRigidbody
{
    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;

    public int id;

    public MotiveRigidbody(Vector3 pos, Quaternion rot, int id)
    {
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;

        rotX = rot.x;
        rotY = rot.y;
        rotZ = rot.z;
        rotW = rot.w;

        this.id = id;
    }
}
