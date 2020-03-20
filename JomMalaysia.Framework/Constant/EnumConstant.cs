namespace JomMalaysia.Framework.Constant
{
    public class EnumConstant
    {
        public enum PublishStatusEnum
        {
            Pending,
            Published,
            Unpublished
        }

        public enum WorkflowStatusEnum
        {
            All,
            Pending,
            Level1,
            Level2,
            Completed,
            Rejected
        }

        public enum WorkflowTypeEnum
        {
            Publish,
            Edit,
            Unpublish,
            Delete
        }

        public enum WorkflowActionEnum
        {
            Approve,
            Reject
        }
    }
}