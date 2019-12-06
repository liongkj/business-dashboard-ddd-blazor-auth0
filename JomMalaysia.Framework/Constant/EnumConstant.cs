using System;
using System.Collections.Generic;
using System.Text;

namespace JomMalaysia.Framework.Constant
{
    public class EnumConstant
    {

        public enum PublishStatusEnum
        {
            pending,
            published,
            unpublished
        }

        public enum WorkflowStatusEnum
        {
            all,
            pending,
            level1,
            level2,
            completed,
            rejected
        }

        public enum WorkflowTypeEnum
        {
            publish,
            edit,
            unpublish,
            delete
        }

        public enum WorkflowActionEnum
        {
            approve,
            reject
        }


    }
}
