import AppReducer from './appReducer';
import {combineReducers} from 'redux';
let Reducers = combineReducers(
    {
        appState: AppReducer
    }
);
export default Reducers;
