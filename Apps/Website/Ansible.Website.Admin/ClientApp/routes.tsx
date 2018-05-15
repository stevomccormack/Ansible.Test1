import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { VoteListing } from './components/vote/Listing';
import { EditVote } from './components/vote/Edit';

export const routes = <Layout>
    <Route exact path='/vote' component={ VoteListing } />
    <Route path='/vote/edit/:id' component={ EditVote } />
</Layout>;
