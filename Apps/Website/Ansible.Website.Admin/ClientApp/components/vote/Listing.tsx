import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import { IVote } from '../../models/Vote';

interface IFetchVotesDataState {
    votes: IVote[];
    loading: boolean;
    error: string;
}

export class VoteListing extends React.Component<RouteComponentProps<{}>, IFetchVotesDataState> {

    constructor() {
        super();
        this.state = { votes: [], loading: true, error: '' };

        //const url = 'http://localhost:51000/odata/Vote?$format=application/json;odata.metadata=none';
        const url = 'http://localhost:51000/odata/Vote';

        fetch(url)
            .then(response => {
                if (!response.ok) {
                    this.setState({ votes: [], loading: false, error: response.statusText });
                }
                return response.json();
                //return response.json() as Promise<IVote[]>;
            })
            .then(json => {

                //TODO: OData causing issues - must be a better way to do this.

                const votes: IVote[] = [];
                if (json.value && json.value.length > 0) {
                    for (let i = 0; i < json.value.length; i++) {
                        const item = json.value[i];
                        const vote: IVote = {
                            voteId: item.VoteId,
                            voteNumber: item.VoteNumber,
                            givenName: item.GivenName,
                            surname: item.Surname,
                            gender: item.Gender,
                            age: item.Age,
                            isValid: item.IsValid
                        };
                        votes.push(vote);
                    }
                }

                this.setState({ votes: votes, loading: false, error: '' });
            })
            .catch(reason => {

                this.setState({ votes: [], loading: false, error: `Failed fetching data. Error: ${reason}` });
                console.log(`Failed fetching data. Error: ${reason}`);
            });
    }

    public render() {
        const contents = this.state.loading ? <p><em>Loading...</em></p> : (this.state.error !== '' ? <p>Failed: ${this.state.error}</p> : VoteListing.renderVotesTable(this.state.votes));

        return <div>

            <h2>Vote Information</h2>
            {contents}

        </div>;
    }

    private static createEditLink(voteId: number): string {
        return `/vote/edit/${voteId}`;
    }

    private static renderVotesTable(votes: IVote[]) {
        const rows = votes.map((vote) =>
            <tr key={vote.voteId}>
                <td className="text-center"><a href={`/vote/edit/${vote.voteId}`}>{vote.voteId}</a></td>
                <td className="text-center"><a href={`/vote/edit/${vote.voteId}`}>{vote.voteNumber}</a></td>
                <td className="text-left">{vote.givenName} {vote.surname} | Age: {vote.age}</td>
                <td className="text-center">{vote.isValid ? 'true' : 'false'}</td>
            </tr>
        );

        return <div className="table-responsive">
            <table className="table table-striped table-hover votes-table">
                <thead>
                    <tr>
                        <th className="text-center">Vote Id</th>
                        <th className="text-center">Vote Number</th>
                        <th className="text-left">Voter Description</th>
                        <th className="text-center">Valid?</th>
                    </tr>
                </thead>
                <tbody>
                    {rows}
                </tbody>
            </table>
        </div>;
    }
}