using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToMesh : MonoBehaviour {

	[ContextMenu("ConvertToRegualrMesh")]
	void Convert() {
		SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer> ();
		MeshRenderer meshrenderer = gameObject.AddComponent<MeshRenderer> ();
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter> ();

		meshFilter.sharedMesh = skinnedMeshRenderer.sharedMesh;
		meshrenderer.sharedMaterials = skinnedMeshRenderer.sharedMaterials;

		DestroyImmediate (skinnedMeshRenderer);
		DestroyImmediate (this);
	}
}
