import * as React from 'react';
import * as ReactDOM from 'react-dom';
import AppContainer from './appContainer';
import {Provider} from 'react-redux';
import Store from './appStore';
window.onload = () => {
    ReactDOM.render(
        <Provider store={Store} >
            <AppContainer />
        </Provider>,
        document.querySelector('main')
    );
}