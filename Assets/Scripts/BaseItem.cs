using UnityEngine;

namespace FPT.BaseEntities
{
    public abstract class BaseItem : MonoBehaviour
    {
        public string Name { get => _name; }
        private string _name;

        public string Description { get => _description; }
        private string _description;

        public string Icon { get => _icon; set => _icon = value; }
        private string _icon;
    }
}
