using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringListEvent:UnityEvent<List<string>>{
	
}

[System.Serializable]
public class FloatListEvent:UnityEvent<List<float>>{
	
}

[System.Serializable]
public class FloatEvent:UnityEvent<float>{
	
}

[System.Serializable]
public class IntEvent:UnityEvent<int>{
	
}
[System.Serializable]
public class BoolEvent:UnityEvent<bool>{
	
}

[System.Serializable]
public class StringEvent:UnityEvent<string>{
	
}
[System.Serializable]
public class IntBoolEvent : UnityEvent<int,bool>{
	
}

[System.Serializable]
public class AudioClipEvent : UnityEvent<AudioClip>{
	
}

//[System.Serializable]
//public class GameObjectEvent : UnityEvent<GameObject>{
	
//}
[System.Serializable]
public class GameObjectListEvent : UnityEvent<List<GameObject>>{
	
}

[System.Serializable]
public class TransformEvent : UnityEvent<Transform>{
	
}

[System.Serializable]
public class TransformListEvent : UnityEvent<List<Transform>>{
	
}

[System.Serializable]
public class Vector3Event : UnityEvent<Vector3>{
	
}
[System.Serializable]
public class FloatStringEvent : UnityEvent<string,float>
{

}

[System.Serializable]
public class Texture2DEvent : UnityEvent<Texture2D>
{

}

[System.Serializable]
public class TextureEvent : UnityEvent<Texture>
{

}

[System.Serializable]
public class AudiosourceEvent : UnityEvent<AudioSource>
{

}

[System.Serializable]
public class IEnumeratorEvent: UnityEvent<IEnumerator>{
	
}
[System.Serializable]
public class GameObjectVector3Event : UnityEvent<GameObject,Vector3>{
	
}
[System.Serializable]
public class StringStringEvent : UnityEvent<string,string>{
	
}
[System.Serializable]
public class StringStringDictionaryEvent : UnityEvent<Dictionary<string,string>>{
	
}
[System.Serializable]
public class ObjectEvent : UnityEvent<object>{
	
}
[System.Serializable]
public class StringFloatEvent:UnityEvent<string,float>{
	
}
[System.Serializable]
public class ColorEvent:UnityEvent<Color>{
	
}
[System.Serializable]
public class RaycastHitEvent:UnityEvent<RaycastHit>{
	
}
[System.Serializable]
public class Vector3ListEvent:UnityEvent<List<Vector3>>{
	
}
[System.Serializable]
public class SpriteRendererEvent:UnityEvent<SpriteRenderer>{}
[System.Serializable]
public class SpriteEvent:UnityEvent<Sprite>{}
[System.Serializable]
public class AnimatorControllerEvent:UnityEvent<RuntimeAnimatorController>{}
[System.Serializable]
public class ScriptableObjectEvent:UnityEvent<ScriptableObject>{}
[System.Serializable]
public class SpriteListEvent:UnityEvent<List<Sprite>>{}