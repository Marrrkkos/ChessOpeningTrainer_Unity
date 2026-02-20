using UnityEngine;
using UnityEngine.UI;


public class SnapPreview : MonoBehaviour
{
    public Camera previewCam;

    public Texture2D TakePhoto()
    {
        // Wir rufen die Funktion auf (Breite/Höhe z.B. 256x256)
        return CaptureScreenshot(previewCam, 512, 512);
        
    }

    // Die Funktion von oben hier einfügen...
    public Texture2D CaptureScreenshot(Camera cam, int width, int height) {
        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();
        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        return screenShot;
    }
}
