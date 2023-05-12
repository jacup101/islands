using UnityEngine;

public class Harvesting : MonoBehaviour
{

   public bool notClient = false;
   public Tool EquippedTool 
   {
      get
      {
         return _tool;
      }

      set
      {
         if(_tool != value)
            {
               _tool = value;
               
               //Update sprite
               UpdateSprite();

            }
         }
   }

   private void UpdateSprite()
   {
      if (_tool != null)
      {
         if(_tool.ToString() == "Tool_Weapon_Copper (Tool)")
         {
            _renderer.flipX = true;
         }
         _renderer.flipX = false;
         _renderer.sprite = _tool.Sprite;
      }
      else
      {
         _renderer.sprite = null;
      }

   }

    [SerializeField] private Tool _tool;
   private SpriteRenderer _renderer;

   private void Start()
   {
      _renderer = GetComponent<SpriteRenderer>();
      UpdateSprite();
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (notClient) {
         return;
      }
      Harvestable harvestable = collision.GetComponent<Harvestable>();
      
      if(harvestable != null)
      {
         int amountToHarvest = UnityEngine.Random.Range(EquippedTool.MinHarvest, EquippedTool.MaxHarvest);
         harvestable.TryHarvest(EquippedTool.Type, amountToHarvest);
      }
   }

   public void ChangeTool(Tool tool)
   {
      this.EquippedTool = tool;
   }

   public int GetToolType() {
      if (_tool == null) {
         return -1;
      }
      // print(_tool.ToString());
      if (_tool.ToString().Contains("Tool_Weapon_Copper")) {
         return 0;
      }
      if (_tool.ToString().Contains("Tool_Axe_Copper")) {
         return 1;
      }
      if (_tool.ToString().Contains("Tool_Pickaxe_Copper")) {
         return 2;
      }
      if (_tool.ToString().Contains("Tool_Bridge_Hammer")) {
         return 3;
      }
      if (_tool.ToString().Contains("Tool_Weapon_Iron")) {
         return 4;
      }
      if (_tool.ToString().Contains("Tool_Axe_Iron")) {
         return 5;
      }
      if (_tool.ToString().Contains("Tool_Pickaxe_Iron")) {
         return 6;
      }
      if (_tool.ToString().Contains("Tool_Weapon_Gold")) {
         return 7;
      }
      if (_tool.ToString().Contains("Tool_Axe_Gold")) {
         return 8;
      }
      if (_tool.ToString().Contains("Tool_Pickaxe_Gold")) {
         return 9;
      }
      return -1;
   }
}
