using UnityEngine;
 
public class CariFile : MonoBehaviour {
 
	protected string m_textPath;
 
	protected FileBrowser m_fileBrowser;
 
	[SerializeField]
	protected Texture2D	m_directoryImage,
						m_fileImage;
 
	protected void OnGUI () {
		if (m_fileBrowser != null) {
			m_fileBrowser.OnGUI();
		} else {
			OnGUIMain();
		}
	}
 
	protected void OnGUIMain() {
 
		GUILayout.BeginHorizontal();
			GUILayout.Label("File Pendukung :", GUILayout.Width(100));
			GUILayout.FlexibleSpace();
			GUILayout.Label(m_textPath ?? "belum dipilih");
			if (GUILayout.Button("...", GUILayout.ExpandWidth(false))) {
				m_fileBrowser = new FileBrowser(new Rect(100, 100, 600, 500),"Pilih File",FileSelectedCallback);
				m_fileBrowser.SelectionPattern = "*.*";
				m_fileBrowser.DirectoryImage = m_directoryImage;
				m_fileBrowser.FileImage = m_fileImage;
			}
		//GUI.Button()
		GUILayout.EndHorizontal();
	}
 
	protected void FileSelectedCallback(string path) {
		m_fileBrowser = null;
		m_textPath = path;
	}
}