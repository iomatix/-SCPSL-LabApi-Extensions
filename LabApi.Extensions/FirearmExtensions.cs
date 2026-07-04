using InventorySystem.Items.Firearms.Attachments;
using LabApi.Features.Wrappers;

namespace LabApi.Extensions
{
    /// <summary>
    /// Provides high-performance fluent utility extensions for validating firearm equipment properties and tactical component arrays.
    /// </summary>
    internal static class FirearmExtensions
    {
        /// <summary>
        /// Evaluates defensively whether the specified firearm instance contains an active attachment matching a designated blueprint identity token.
        /// </summary>
        /// <param name="firearm">The target <see cref="FirearmItem"/> instance whose attachments array undergoes validation.</param>
        /// <param name="attachmentName">The structural identity name token of the component to locate.</param>
        /// <returns><c>true</c> if the attachment exists, is fully initialized, and actively enabled; otherwise, <c>false</c>.</returns>
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
    }
}