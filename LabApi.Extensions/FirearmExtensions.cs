using InventorySystem.Items.Firearms.Attachments;
using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Utility extensions for checking firearm attachments.
    /// </summary>
    public static class FirearmExtensions
    {
        #region Single Firearm Checks

        /// <summary>
        /// Returns true if the firearm has the specified attachment enabled.
        /// </summary>
        public static bool HasAttachment(this FirearmItem firearm, AttachmentName attachmentName)
        {
            if (firearm?.Base?.Attachments is null)
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
        /// </summary>
        public static bool HasAttachments(this FirearmItem firearm, IEnumerable<AttachmentName> attachmentNames)
        {
            if (firearm?.Base?.Attachments is null || attachmentNames is null)
                return false;

            foreach (var name in attachmentNames)
            {
                if (!firearm.HasAttachment(name))
                    return false;
            }

            return true;
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
            if (firearms is null)
                return false;

            if (firearms is List<FirearmItem> list)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!list[i].HasAttachment(attachmentName))
                        return false;
                }
                return true;
            }

            foreach (var firearm in firearms)
            {
                if (!firearm.HasAttachment(attachmentName))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if all firearms in the params array have the specified attachment enabled.
        /// </summary>
        public static bool HasAttachment(AttachmentName attachmentName, params FirearmItem[] firearms)
            => ((IEnumerable<FirearmItem>)firearms).HasAttachment(attachmentName);

        #endregion
    }
}
