import * as React from 'react';
import SideBar from './components/sidebar/SideBar';
import TopBar from './components/topBar/TopBar';
import {SideBarOption} from './models/option'
import Icons from './components/icons/Icons';
import SearchBar from './components/searchBar/SearchBar';
import {SideBarState} from './enums';
import DataService from './services/dataService';
import Messages from './components/messages/Messages';
interface IAppState {
    searchValue: string;
    sideBarState: SideBarState;
    categories: any[];
    path?: string;
}

interface IAppDispatch {
}

export default class AppContainer extends React.Component<any, IAppState> {
    db = new DataService(`ws://${location.host}/WebSocket/Sock`, 'pneumail')
    constructor(props) {

        super(props);
        this.state = {
            searchValue: '',
            sideBarState: SideBarState.Open,
            categories: [] as any[],
            path: '/', 
        }
        this.db.listen(categories => {
            this.setState((prev, props) => {
                return {
                    categories
                }
            });
        });
        
        window.addEventListener('popstate', ev => {
            console.log('window.popstate', ev);
        });
    }

    navigationClicked(href: string) {
        console.log('navigationClicked', href);
    }

    componentWillMount() {
        // console.log('appContainer', 'componentWillMount', this.state)
    }
    componentDidMount() {
        // console.log('appContainer', 'componentDidMount', this.state)
    }
    componentWillReceiveProps(props) {
        // console.log('appContainer', 'componentWillReceiveProps', props)
    }

    componentWillUpdate(props) {
        // console.log('appContainer', 'componentWillUpdate', this.state)
    }
    componentDidUpdate() {
        // console.log('appContainer', 'componentDidUpdate', this.state)
    }
    componentWillUnmount() {
        // console.log('appContainer', 'componentWillUnmount', this.state)
    }
    render() {
        return (
            <div id="app-container">
                <TopBar
                    bannerText="Pneumail"
                    searchTextUpdate={(value) => this.updateSearch(value)}
                />
                <div className="main-content">
                    <SideBar
                        title="Collections"
                        options={[
                            new SideBarOption('Inbox', '/', Icons.InboxIcon),
                            new SideBarOption('Sent', '/sent', Icons.SentIcon),
                            new SideBarOption('Trips', '/trips', Icons.TripsIcon),
                            new SideBarOption('Settings', '/Account/Settings')
                        ]}
                        width={this.state.sideBarState}
                        toggleWidth={() => this.toggleSidebar()}
                        elementClicked={href => this.navigationClicked(href)}
                    />
                    {this.state.categories.length > 0 ? <Messages
                        title={this.state.categories[0].name}
                        messages={this.state.categories[0].messages}
                        expanded={this.state.sideBarState == SideBarState.Closed}
                    /> : null}
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