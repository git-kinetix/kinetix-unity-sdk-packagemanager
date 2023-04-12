using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinetix.Editor
{
    public interface IEditorWindowComponent
    {
        void Draw(Rect position);
    }
}

