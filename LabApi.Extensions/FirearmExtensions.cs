using InventorySystem.Items.Firearms.Attachments;
using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Highly optimized utility extensions for checking firearm attachments.
    /// Reuses state-passing query extensions to maintain a zero-allocation, short-circuiting execution path.
    /// </summary>
    public static class FirearmExtensions
    {
        #region Single Firearm Checks

        /// <summary>
        /// Returns true if the firearm has the specified attachment enabled.
        /// </summary>
        public static bool HasAttachment(this FirearmItem firearm, AttachmentName attachmentName)
        {
            if (firearm == null || firearm.Base == null || firearm.Base.Attachments == null)
                return false;

            var attachments = firearm.Base.Attachments;
            int count = attachments.Length;

            for (int i = 0; i < count; i++)
            {
                var a = attachments[i];
                if (a != null && a.Name == attachmentName && a.IsEnabled)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the firearm has all specified attachments enabled.
        /// Uses state-passing All helper to guarantee 0 allocations and instant early-exit.
        /// </summary>
        public static bool HasAttachments(this FirearmItem firearm, IEnumerable<AttachmentName> attachmentNames)
        {
            if (firearm == null || firearm.Base == null || firearm.Base.Attachments == null || attachmentNames == null)
                return false;

            // FIX: DRY & zero-allocation early-exit query using our new All extension.
            return attachmentNames.All(firearm, static (name, f) => f.HasAttachment(name));
        }

        /// <summary>
        /// Returns true if the firearm has all specified attachments enabled (params overload).
        /// </summary>
        public static bool HasAttachments(this FirearmItem firearm, params AttachmentName[] attachmentNames)
            => firearm.HasAttachments((IEnumerable<AttachmentName>)attachmentNames);

        #endregion

        #region Batch Firearm Checks

        /// <summary>
        /// Returns true if all firearms in the collection have the specified attachment enabled.
        /// </summary>
        public static bool HasAttachment(this IEnumerable<FirearmItem> firearms, AttachmentName attachmentName)
        {
            if (firearms == null)
                return false;

            // FIX: DRY & zero-allocation early-exit batch query.
            return firearms.All(attachmentName, static (f, name) => f != null && f.HasAttachment(name));
        }

        /// <summary>
        /// Returns true if all firearms in the params array have the specified attachment enabled.
        /// </summary>
        public static bool HasAttachment(AttachmentName attachmentName, params FirearmItem[] firearms)
            => ((IEnumerable<FirearmItem>)firearms).HasAttachment(attachmentName);

        #endregion
    }
}