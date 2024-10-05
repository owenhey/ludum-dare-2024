using System;
using System.Collections.Generic;
using UnityEngine;

public class RenderWordsPool : MonoBehaviour {
    public List<RenderWord> objects;

    private static RenderWordsPool _instance;

    private void Awake() {
        _instance = this;
    }

    public static RenderWord Get() {
        for (int i = 0; i < _instance.objects.Count; i++) {
            if (!_instance.objects[i].gameObject.activeInHierarchy) {
                return _instance.objects[i];
            }
        }

        var newobj = Instantiate(_instance.objects[0]);
        newobj.gameObject.SetActive(false);
        _instance.objects.Add(newobj);
        return newobj;
    }
}
