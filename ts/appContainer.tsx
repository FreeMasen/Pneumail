import * as React from 'react';
import ExpandToggle from './buttons/expandToggle';

export default class AppContainer extends React.Component<any, any> {
    render() {
        return (
            <div id="app-container">
                <ExpandToggle />
            </div>
        )
    }
}