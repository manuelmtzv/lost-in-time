using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{

    [SerializeField] float verticalSpeed = 3.5f;
    [SerializeField] float lifeTime = 0.3f;
    [SerializeField] float fadeTime = 5f;
    [SerializeField] Color normalDamageColor = Color.white;
    [SerializeField] Color critDamageColor = Color.red;

    private TextMeshPro textMesh;
    private Color textColor;
    private static int sortingOrder;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public static DamagePopup Generate(Vector3 position, int damageAmount, bool critDamage = false)
    {
        GameObject damagePopup = Instantiate(GlobalAssets.Instance.damagePopupPrefab, position, Quaternion.identity);
        DamagePopup damagePopupScript = damagePopup.GetComponent<DamagePopup>();
        damagePopupScript.ConfigurePopup(damageAmount, critDamage);
        return damagePopupScript;
    }

    private void ConfigurePopup(int damageAmount, bool critDamage)
    {
        textMesh.SetText(damageAmount.ToString());

        if (critDamage)
        {
            textMesh.fontSize = 3;
            textColor = critDamageColor;
        }
        else
        {
            textColor = normalDamageColor;
        }

        textMesh.color = textColor;
        textMesh.sortingOrder = sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
    }

    public void Update()
    {
        transform.position += new Vector3(0, verticalSpeed) * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        float alpha = textColor.a;

        if (lifeTime <= 0)
        {
            textColor.a -= fadeTime * Time.deltaTime;
            textMesh.color = textColor;

            if (alpha <= 0) Destroy(gameObject);
        }
    }
}
