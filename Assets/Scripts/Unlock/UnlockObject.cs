using Assets.Scripts.Currency;
using Assets.Scripts.Items;
using Assets.Scripts.Utiltities.ScriptableObjects;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Unlock
{
    public class UnlockObject : MonoBehaviour
    {
        [SerializeField] UnlockableItem itemSo;
        [SerializeField] TextMeshProUGUI priceText; 

        private UnlockService unlockService;

        private void Start()
        {
            priceText.text = "$ " + itemSo.unlockCost;
        }
        public void SetService(UnlockService unlockService)
        {
            this.unlockService = unlockService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Dollar currency = unlockService.currencyService.dollar;
                if (itemSo.unlockCost <= currency.GetCurrentBalance())
                {
                    GameObject i = Instantiate(itemSo.prefab, transform.parent.position, quaternion.identity);
                    currency.SubtractCurrency(itemSo.unlockCost);
                    i.transform.DOScale(1f, 1f).SetEase(Ease.OutElastic);
                    gameObject.SetActive(false);
                    unlockService.eventService.OnItemUnlock.Invoke(i.GetComponentInChildren<Shelf>());
                }
                else
                {
                    Debug.Log("not enough balance");
                }
            }
        }
    }
}