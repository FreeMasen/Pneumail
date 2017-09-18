import * as React from 'react';
import SideBar from './sidebar/SideBar';
import TopBar from './topBar/TopBar';

export default class AppContainer extends React.Component<any, any> {
    render() {
        return (
            <div id="app-container">
                <TopBar
                    bannerText="Pneumail"
                />
                <SideBar
                    expanded={false}
                    title="Collections"
                />
            </div>
        )
    }
}