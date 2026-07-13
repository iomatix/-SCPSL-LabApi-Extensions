using InventorySystem.Items.Firearms.Attachments;
using LabApi.Features.Wrappers;
using System.Collections.Generic;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides extension methods for validating firearm attachments and equipment properties.
    /// </summary>
    public static class FirearmExtensions
    {
        #region Single Firearm Checks
        /// <summary>
        /// Checks if the firearm has the specified attachment enabled.
        /// </summary>
        public static bool HasAttachment(this FirearmItem firearm, AttachmentName attachmentName)
        {
            if (firearm?.Base?.Attachments is null) return false;

            var attachments = firearm.Base.Attachments;
            int count = attachments.Length;

            for (int i = 0; i < count; i++)
            {
                var attachment = attachments[i];
                if (attachment is not null && attachment.Name == attachmentName && attachment.IsEnabled)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the firearm has all specified attachments enabled.
        /// </summary>
        public static bool HasAttachments(this FirearmItem firearm, IEnumerable<AttachmentName> attachmentNames)
        {
            if (firearm is null || attachmentNames is null) return false;

            if (attachmentNames is List<AttachmentName> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!firearm.HasAttachment(concreteList[i])) return false;
                }
                return true;
            }

            foreach (AttachmentName name in attachmentNames)
            {
                if (!firearm.HasAttachment(name)) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if the firearm has all inline array attachments enabled.
        /// </summary>
        public static bool HasAttachments(this FirearmItem firearm, params AttachmentName[] attachmentNames)
        {
            if (firearm is null || attachmentNames is null) return false;

            int count = attachmentNames.Length;
            for (int i = 0; i < count; i++)
            {
                if (!firearm.HasAttachment(attachmentNames[i])) return false;
            }

            return true;
        }
        #endregion

        #region Batch Firearm Collections Checks (Added for API Consistency)
        /// <summary>
        /// Checks if all firearms in the collection have the specified attachment enabled.
        /// </summary>
        public static bool HasAttachment(this IEnumerable<FirearmItem> firearms, AttachmentName attachmentName)
        {
            if (firearms is null) return false;

            if (firearms is List<FirearmItem> concreteList)
            {
                int count = concreteList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (!concreteList[i].HasAttachment(attachmentName)) return false;
                }
                return true;
            }

            foreach (FirearmItem firearm in firearms)
            {
                if (!firearm.HasAttachment(attachmentName)) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if all inline array firearms have the specified attachment enabled.
        /// </summary>
        public static bool HasAttachment(AttachmentName attachmentName, params FirearmItem[] firearms)
        {
            if (firearms is null) return false;

            int count = firearms.Length;
            for (int i = 0; i < count; i++)
            {
                if (!firearms[i].HasAttachment(attachmentName)) return false;
            }

            return true;
        }
        #endregion
    }
}