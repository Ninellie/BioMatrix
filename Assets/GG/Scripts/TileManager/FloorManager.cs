//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FloorManager : MonoBehaviour
//{
//    int height = 1024;
//    int width = 1024;
//    int depth = 24;
//    public Camera camera;
//    public GameObject go;
//    public Sprite spriteOfScreen;
//    public SpriteRenderer m_SpriteRenderer;

//    private void Start()
//    {
//        go = GameObject.FindGameObjectWithTag("Player");

//        m_SpriteRenderer = GetComponent<SpriteRenderer>();

//        Invoke("ChangeSprite", 1);
//    }

//    public void ChangeSprite()
//    {
//        Debug.Log("Creating Sprite");
//        spriteOfScreen = CaptureScreen();
//        Invoke("ChangeSprite", 1);

//    }
//    public Sprite CaptureScreen()
//    {

//        RenderTexture renderTexture = new RenderTexture(width, height, depth);
//        camera.targetTexture = renderTexture;

//        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
//        Rect rect = new Rect(0, 0, width, height);

//        camera.Render();

//        RenderTexture currentRenderTexture = RenderTexture.active;
//        RenderTexture.active = renderTexture;
//        texture.ReadPixels(rect, 0, 0);
//        texture.Apply();

//        //reset camera's target texture
//        camera.targetTexture = null;
//        //reset active renderTexture
//        RenderTexture.active = currentRenderTexture;
//        //free some memory
//        Destroy(renderTexture);
//        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

//        return sprite;
//    }
//}
