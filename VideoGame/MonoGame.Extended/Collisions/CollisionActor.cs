using Microsoft.Xna.Framework;
using MonoGame.Extended.Shapes;

namespace MonoGame.Extended.Collisions
{
    public class CollisionActor : IActorTarget
    {
        public CollisionActor(IActorTarget target)
        {
            _target = target;
        }

        private readonly IActorTarget _target;

        public Vector2 Velocity
        {
            get { return _target.Velocity; }
            set { _target.Velocity = value; }
        }

        public Vector2 Position
        {
            get { return _target.Position; }
            set { _target.Position = value; }
        }

        public RectangleF BoundingBox
        {
            get { return _target.BoundingBox; }
        }

        public void OnCollision(CollisionInfo collisionInfo)
        {
            _target.OnCollision(collisionInfo);
        }
    }
}