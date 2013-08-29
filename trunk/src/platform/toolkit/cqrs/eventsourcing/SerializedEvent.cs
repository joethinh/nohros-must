using System;

namespace Nohros.CQRS.EventSourcing
{
  public class SerializedEvent
  {
    readonly byte[] data_;
    readonly byte[] metadata_;

    #region .ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedEvent"/> class
    /// by using the serialized event's metadata and data.
    /// </summary>
    /// <param name="data">
    /// An array of bytes containing the event data.
    /// </param>
    public SerializedEvent(byte[] data) {
      metadata_ = new byte[0];
      data_ = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SerializedEvent"/> class
    /// by using the serialized event's metadata and data.
    /// </summary>
    /// <param name="metadata">
    /// An array of bytes containing metadata about the event.
    /// </param>
    /// <param name="data">
    /// An array of bytes containing the event data.
    /// </param>
    public SerializedEvent(byte[] metadata, byte[] data) {
      metadata_ = metadata;
      data_ = data;
    }
    #endregion

    /// <summary>
    /// Gets an array of bytes containing metadata about the event.
    /// </summary>
    public byte[] Metadata {
      get { return metadata_; }
    }

    /// <summary>
    /// Gets an array of bytes containig the event data.
    /// </summary>
    public byte[] Data {
      get { return data_; }
    }
  }
}
