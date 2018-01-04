// http://baba-s.hatenablog.com/entry/2015/05/16/115549

using UnityEditor;
using UnityEngine;

public static class ProjectViewLiner
{
    [InitializeOnLoadMethod]
    private static void Example()
    {
        EditorApplication.projectWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI( string guid, Rect selectionRect )
    {
        var index = ( int )( selectionRect.y - 4 ) / 16;

        if ( index % 2 == 0 )
        {
            return;
        }

        var pos     = selectionRect;
        pos.x       = 0;
        pos.xMax    = selectionRect.xMax;

        var color = GUI.color;
        GUI.color = new Color( 0, 0, 0, 0.1f );
        GUI.Box( pos, string.Empty );
        GUI.color = color;
    }
}
