using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Views.UI.Features.Machines
{
    public class MachinesPanelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _machinesContainer;
        [SerializeField] private ScrollRect _scrollRect;
        
        private List<MachineView> _machineViews = new List<MachineView>();
        
        public void AddMachineView(MachineView machineView)
        {
            if (!_machineViews.Contains(machineView))
            {
                _machineViews.Add(machineView);
                
                if (_machinesContainer != null && machineView != null)
                {
                    machineView.transform.SetParent(_machinesContainer, false);
                }
            }
        }
        
        public void RemoveMachineView(MachineView machineView)
        {
            if (_machineViews.Contains(machineView))
            {
                _machineViews.Remove(machineView);
            }
        }
        
        public void ArrangeMachines()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);
        }
    }
}