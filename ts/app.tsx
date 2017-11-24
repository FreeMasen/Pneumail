import * as React from 'react';
import * as ReactDOM from 'react-dom';
import AppContainer from './appContainer';
import {Provider} from 'react-redux';

window.onload = () => {
    ReactDOM.render(
        <AppContainer />,
        document.querySelector('main')
    );
}