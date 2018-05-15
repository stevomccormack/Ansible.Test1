import * as React from 'react';
import { Route } from 'react-router-dom';
import { RouteComponentProps } from 'react-router';
import 'isomorphic-fetch';
import { IVote } from '../../models/Vote';

interface IFetchVoteDataState {
    vote: IVote;
    loading: boolean;
    error: string;
}

export class EditVote extends React.Component<RouteComponentProps<{}>, IFetchVoteDataState> {

    constructor() {
        super();

        let initVote = {
            voteId: 0,
            voteNumber: '',
            givenName: '',
            surname: '',
            gender: '',
            age: 0,
            isValid: false
        };

        this.state = { vote: initVote, loading: true, error: '' };

        // When the URL is /the-path?some-key=a-value ...
        const query = new URLSearchParams(location.search);
        const voteId = query.get('id');

        //const url = `http://localhost:51000/odata/Vote(${voteId})?$format=application/json;odata.metadata=none`;
        const url = `http://localhost:51000/odata/Vote(${voteId})`;

        fetch(url)
            .then(response => {
                if (!response.ok) {
                    this.setState({ vote: initVote, loading: false, error: response.statusText });
                }
                return response.json();
                //return response.json() as Promise<IVote>;
            })
            .then(json => {
                const item = json;
                const vote: IVote = {
                    voteId: item.VoteId,
                    voteNumber: item.VoteNumber,
                    givenName: item.GivenName,
                    surname: item.Surname,
                    gender: item.Gender,
                    age: item.Age,
                    isValid: item.IsValid
                };

                this.setState({ vote: vote, loading: false, error: '' });
            })
            .catch(reason => {

                this.setState({ vote: initVote, loading: false, error: `Failed fetching data. Error: ${reason}` });
                console.log(`Failed fetching data. Error: ${reason}`);
            });
    }

    public render() {
        let contents = this.state.loading ? <p><em>Loading...</em></p> : (this.state.error !== '' ? <p>Failed: ${this.state.error}</p> : EditVote.renderVoteForm(this.state.vote));

        return <div>

            <h2>Edit Vote</h2>
            {contents}

        </div>;

    }

    private static renderVoteForm(vote: IVote) {
        return <div className="form-container">
            <form className="form-horizontal" method="PUT" action="http://localhost:51000/odata/Vote">

                <input type="hidden" id="voteId" aria-describedby="voteId" value={vote.voteId} />

                <div className="form-group">
                    <label>Vote Number</label>
                    <input type="text" className="form-control input-lg" id="voteNumber" aria-describedby="voteNumber" placeholder="Vote Number" value={vote.voteNumber} />
                    <small className="form-text text-muted">The vote number is auto generated.</small>
                </div>
                <div className="row">
                    <div className="col-sm-6">
                        <div className="form-group">
                            <label>Given Name</label>
                            <input type="text" className="form-control input-lg" id="givenName" aria-describedby="givenName" placeholder="Given Name" value={vote.givenName} />
                        </div>
                    </div>
                    <div className="col-sm-6">
                        <div className="form-group ">
                            <label>Surname</label>
                            <input type="text" className="form-control input-lg" id="surname" aria-describedby="surname" placeholder="Surname" value={vote.surname} />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-sm-6">
                        <div className="form-group">
                            <label>Gender</label>
                            <input type="text" className="form-control input-lg" id="gender" aria-describedby="gender" placeholder="Gender" value={vote.gender} />
                        </div>
                    </div>
                    <div className="col-sm-6">
                        <div className="form-group">
                            <label>Age</label>
                            <input type="number" className="form-control input-lg" id="age" aria-describedby="age" placeholder="Age (in years)" value={vote.age} />
                        </div>
                    </div>
                </div>
                <div className="form-group">
                    <label>Is Valid?</label>
                    <input type="checkbox" className="checkbox-inline" id="isValid" aria-describedby="isValid" checked />
                </div>

                <button type="submit" className="btn btn-lg btn-primary">Submit</button>

            </form>
        </div>;
    }
}