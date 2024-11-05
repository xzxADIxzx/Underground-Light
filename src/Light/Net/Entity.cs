namespace Light.Net;

using Godot;
using Light.Content;
using Light.IO;
using Light.Singletons;

/// <summary> Entities are synchronized across the network by means of writing and reading snapshots. </summary>
public abstract partial class Entity : Node
{
    /// <summary> Unique identifier of the entity. For players, it is equal to their AccountId. </summary>
    public uint Id;
    /// <summary> Type of the entity, for example, a player or some kind of enemy. </summary>
    public EntityType Type;

    /// <summary> AccountId of the entity owner. </summary>
    public uint Owner;
    /// <summary> Whether the local player owns the entity. </summary>
    public bool IsOwner => Owner == Steam.AccId;

    /// <summary> Time of receiving the last snapshot. </summary>
    public float LastUpdate;
    /// <summary> Whether the entity is dead. Dead entities are not synchronized. </summary>
    public bool Dead;

    /// <summary> Adds itself to the entities list. Generates a new id and makes the local player its owner if no id is not provided. </summary>
    public void Init(uint id = uint.MaxValue)
    {
        if (id == uint.MaxValue)
        {
            // Id = Entities.NextId();
            Owner = Steam.AccId;
        }
        Networking.Entities[Id] = this;
    }

    /// <summary> Writes the entity data to the writer. </summary>
    public abstract void Write(Writer w);
    /// <summary> Reads the entity data from the reader. </summary>
    public abstract void Read(Reader r);

    /// <summary> Deals damage to the entity if possible </summary>
    public virtual void Damage(Reader r) { }
    /// <summary> Kills the entity if possible. </summary>
    public virtual void Kill(Reader r) => Dead = true;

    /// <summary> Struct for interpolating floating point numbers. </summary>
    public struct FloatLerp
    {
        /// <summary> Values to be interpolated. </summary>
        public float Last, Target;

        /// <summary> Returns the intermediate value. </summary>
        public readonly float Get(float updateTime) => Mathf.Lerp(Last, Target, (Events.Time - updateTime) * Networking.TICKS_PER_SECOND);

        /// <summary> Returns the intermediate value of the angle. </summary>
        public readonly float GetAngel(float updateTime) => Mathf.LerpAngle(Last, Target, (Events.Time - updateTime) * Networking.TICKS_PER_SECOND);

        public static FloatLerp operator <<(FloatLerp lerp, Reader r) => default(FloatLerp) with
        {
            Last = lerp.Target,
            Target = r.Float()
        };
        public static FloatLerp operator >>(FloatLerp lerp, Writer w)
        {
            w.Float(lerp.Target);
            return lerp;
        }
    }

    /// <summary> Struct for finding entities according to their identifier. </summary>
    public struct EntityProv<T> where T : Entity
    {
        /// <summary> Identifier of the entity to be found. </summary>
        public uint Id;

        private T value;
        public T Value => value ?? (Networking.Entities.TryGetValue(Id, out var e) && e is T t ? value = t : null);

        public static explicit operator uint(EntityProv<T> prov) => prov.Id;
        public static explicit operator EntityProv<T>(uint id) => default(EntityProv<T>) with { Id = id };
    }
}
