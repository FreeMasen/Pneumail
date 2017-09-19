import * as React from 'react';
import SideBar from './sidebar/SideBar';
import TopBar from './topBar/TopBar';
import {SideBarOption} from './models/option'
import Icons from './icons/Icons';
import {BrowserRouter, Route, Link} from 'react-router-dom';
export default class AppContainer extends React.Component<any, any> {
    render() {
        return (
            <BrowserRouter>
                <div id="app-container">
                <TopBar
                    bannerText="Pneumail"
                />
                <SideBar
                    expanded={false}
                    title="Collections"
                    options={[
                        new SideBarOption('Inbox', '/', Icons.InboxIcon),
                        new SideBarOption('Sent', '/sent', Icons.SentIcon)
                    ]}
                />
                <div>

                </div>
                </div>
            </BrowserRouter>
        )
    }
}