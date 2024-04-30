using NetworkGameEngine;
using NetworkGameEngine.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Godless_Lands_Game.ObjectInteraction
{
    internal class InteractionObjectContext
    {
        private GameObject _interactObject = null;
        private List<Component> _interactionComponents = null;
        private GameObject _owner;

        public int InteractObjectId => (_interactObject != null) ? _interactObject.ID : -1;

        public bool HasInteraction => _interactObject != null;

        public GameObject Object => _interactObject;

        public InteractionObjectContext(GameObject gameObject)
        {
            _owner = gameObject;
        }

        public void BeginInteractionWithObject(GameObject interactObject, List<Component> components)
        {
            if (_interactObject != null)
            {
                throw new InvalidOperationException("Cannot start interaction with object while already interacting with another object.");
            }
            _interactObject = interactObject;
            if (_interactionComponents != null)
            {
                throw new InvalidOperationException("Interaction components list is not null when starting interaction with object.");
            }
            _interactionComponents = components;
            foreach (var component in components)
            {
                _owner.AddComponent(component);
            }
        }

        public void EndInteractionWithObject()
        {
            if(_interactObject != null) _interactObject = null;
            else Debug.Log.Warn("Ending interaction with object while not interacting with any object.");

            if (_interactionComponents != null)
            {
                foreach (var component in _interactionComponents)
                {
                    _owner.DestroyComponent(component);
                }
                _interactionComponents = null;
            }
            else Debug.Log.Warn("Interaction components list is null when ending interaction with object.");
        }
    }
}
