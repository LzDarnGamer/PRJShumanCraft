﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class AchivementLog : MonoBehaviour {
    [SerializeField] AudioClip achivementDone;
    [SerializeField] AudioSource sound;
    [SerializeField] GameObject achivementdonePanel;


    [Header("Hidden Achivements Icon")]
    [SerializeField] Sprite dead;

    public int ACHIVEMENT_LEVEL { get; set; }

    private AchivementList achivementList;
    private OrderedDictionary AchivementProgress;

    public Achivement[] keys { get;  private set; }
    public int[] values  { get;  private set; }


    public void Start() {
        achivementList = GameObject.Find("GameSetup").GetComponent<AchivementList>();
        ACHIVEMENT_LEVEL = 0;
        AchivementProgress = new OrderedDictionary();

        for (int i = 0; i < AchivementList.AchivementsList.Count; i++) {
            AchivementProgress.Add(AchivementList.AchivementsList[i], 0);
        }

        ICollection k = AchivementProgress.Keys;
        ICollection v = AchivementProgress.Values;

        keys = new Achivement[AchivementProgress.Count];
        values = new int[AchivementProgress.Count];
        k.CopyTo(keys, 0);
        v.CopyTo(values, 0);

        //achivementdonePanel.transform.localScale = Vector3.zero;
    }

    public void advanceAchivement(ItemObject it) {
        for (int i = 0; i < AchivementProgress.Count; i++) {
            if (!keys[i].isDone && keys[i].requirement[0] == it.itemID) {
                values[i] += 1;
                Debug.Log(it.name + " " + values[i]);
                if (values[i] >= keys[i].requirement[1]) {
                    values[i] = keys[i].requirement[1];
                    ACHIVEMENT_LEVEL++;
                    keys[i].isDone = true;
                    StartCoroutine(achivementAnimation(keys[i]));
                }
            }
        }
    }

    public IEnumerator deadAchivement() {
        for (int i = 0; i < AchivementProgress.Count; i++) {
            if (!keys[i].isDone && keys[i].achName.Equals("You idot")) {
                keys[i].isDone = true;
                sound.PlayOneShot(achivementDone);
                achivementdonePanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "You idot";
                achivementdonePanel.transform.GetChild(1).GetComponent<Image>().sprite = dead;
                LeanTween.scale(achivementdonePanel, new Vector3(1, 1, 1), 0.7f);
                yield return new WaitForSeconds(3f);
                LeanTween.scale(achivementdonePanel, new Vector3(0, 0, 0), 0.4f);
            }
        }

    }


    private IEnumerator achivementAnimation(Achivement ach) {
        sound.PlayOneShot(achivementDone);
        achivementdonePanel.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = ach.achName;
        achivementdonePanel.transform.GetChild(1).GetComponent<Image>().sprite = ach.icon;
        LeanTween.scale(achivementdonePanel, new Vector3(1, 1, 1), 0.7f);
        yield return new WaitForSeconds(3f);
        LeanTween.scale(achivementdonePanel, new Vector3(0, 0, 0), 0.4f);
    }
}
