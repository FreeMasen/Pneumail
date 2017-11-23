import * as React from 'react';
import SideBar from './components/sidebar/SideBar';
import TopBar from './components/topBar/TopBar';
import {SideBarOption} from './models/option'
import Icons from './components/icons/Icons';
import {BrowserRouter, Route, Link} from 'react-router-dom';
import SearchBar from './components/searchBar/SearchBar';
import {SideBarState} from './enums';
interface IAppState {
    searchValue: string;
    sideBarState: SideBarState;
}

interface IAppDispatch {
}

export default class AppContainer extends React.Component<any, IAppState> {
    constructor(props) {
        super(props);
        this.state = {
            searchValue: '',
            sideBarState: SideBarState.Open
        }
    }
    render() {
        return (
            <div id="app-container">
                <TopBar
                    bannerText="Pneumail"
                    searchTextUpdate={(value) => this.updateSearch(value)}
                />
                <SideBar
                    title="Collections"
                    options={[
                        new SideBarOption('Inbox', '/', Icons.InboxIcon),
                        new SideBarOption('Sent', '/sent', Icons.SentIcon),
                        new SideBarOption('Trips', '/trips', Icons.TripsIcon)
                    ]}
                    width={this.state.sideBarState}
                    toggleWidth={() => this.toggleSidebar()}
                />
                <div>

                </div>
            </div>
        )
    }

    toggleSidebar() {
        this.setState((prev: IAppState, props) => {
            return {
                sideBarState: prev.sideBarState == SideBarState.Open ? SideBarState.Closed : SideBarState.Open
            }
        });
    }

    updateSearch(value) {
        this.setState((prev, props) => {
            return {
                value: value
            }
        });
    }
}