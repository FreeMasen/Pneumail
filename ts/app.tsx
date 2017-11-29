import * as React from 'react';
import * as ReactDOM from 'react-dom';
import AppContainer from './appContainer';

window.onload = () => {
    ReactDOM.render(
        <AppContainer />,
        document.querySelector('main')
    );
}