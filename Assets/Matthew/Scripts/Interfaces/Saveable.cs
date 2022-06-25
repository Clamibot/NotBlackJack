using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

/// <summary>
/// This interface inherits from ISerializable, and will be used to save/load objects.<para></para>
/// Requires System.Runtime.Serialization and the [System.Serializable] attribute.
/// </summary>
public interface Saveable : ISerializable
{
	void save(string file);
	void load(string file);

	// inherited from ISerializable:
	//void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context);
	//void MyItemType(SerializationInfo info, StreamingContext context);
}
