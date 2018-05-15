using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ansible.Data.Model;

namespace Ansible.Data.Entity
{

    public interface IVoteManager : IVoteQuery, IVoteCommand
    {

    }

    public interface IVoteQuery: IVoteQueryFind
    {

    }

    public interface IVoteCommand: IVoteCommandUpdate, IVoteCommandSet
    {

    }

    public interface IVoteQueryFind
    {
        #region Unique Queries

        Task<Vote> FindByUniqueVoteNumber(string voteNumber);

        #endregion
    }

    public interface IVoteCommandUpdate
    {
    }

    public interface IVoteCommandSet
    {
        #region Set Date Colummns

        Task<bool> SetModifiedDateTime(int id, DateTimeOffset modifiedDateTime);

        #endregion

        #region Set Bit Colummns

        Task<bool> SetIsValid(int id, bool isValid);

        #endregion
    }
}
