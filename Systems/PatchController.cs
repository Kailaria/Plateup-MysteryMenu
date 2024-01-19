using Kitchen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace KitchenMysteryMenu.Systems
{
    public class PatchController : GameSystemBase
    {
        static PatchController _instance;
        protected override void Initialise()
        {
            base.Initialise();
            _instance = this;
        }

        protected override void OnUpdate()
        {
        }

        internal static Entity StaticCreateEntity(params ComponentType[] types) 
        {
            Entity e = default;
            if (_instance?.EntityManager != null)
            {
                e = _instance.EntityManager.CreateEntity(types);
            }
            return e;
        }

        internal static void StaticAddComponentData<T>(Entity e, T comp) where T : struct, IComponentData
        {
            if (_instance?.EntityManager != null)
            {
                _instance.EntityManager.AddComponentData<T>(e, comp);
            }
        }

        internal static void StaticAddComponent<T>(Entity e) where T : struct, IComponentData
        {
            if (_instance?.EntityManager != null)
            {
                _instance.EntityManager.AddComponent<T>(e);
            }
        }

        internal static void StaticUnlockIngredient(int menuItem, int ingredient)
        {
            _instance?.UnlockIngredient(menuItem, ingredient);
        }

        internal static EntityQuery StaticGetEntityQuery(params ComponentType[] types)
        {
            return _instance?.GetEntityQuery(types) ?? default;
        }

        internal static EntityQuery StaticGetEntityQuery(params EntityQueryDesc[] queryDesc)
        {
            return _instance?.GetEntityQuery(queryDesc) ?? default;
        }
    }
}
