import * as React from 'react';
import SideBar from './components/sidebar/SideBar';
import TopBar from './components/topBar/TopBar';
import {SideBarOption} from './models/option'
import Icons from './components/icons/Icons';
import SearchBar from './components/searchBar/SearchBar';
import {SideBarState} from './enums';
import DataService from './services/dataService';
import Message from './components/message/message';

interface IAppState {
    searchValue: string;
    sideBarState: SideBarState;
    categories: any[];
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
            categories: [] as any[]
        }
        this.db.listen(categories => {
            console.log('listener', categories);
            this.setState((prev, props) => {
                return {
                    categories
                }
            });
        });
    }
    componentWillMount() {
        console.log('appContainer', 'componentWillMount', this.state)
    }
    componentDidMount() {
        console.log('appContainer', 'componentDidMount', this.state)
    }
    componentWillReceiveProps(props) {
        console.log('appContainer', 'componentWillReceiveProps', props)
    }
    
    componentWillUpdate(props) {
        console.log('appContainer', 'componentWillUpdate', this.state)        
    }
    componentDidUpdate() {
        console.log('appContainer', 'componentDidUpdate', this.state)
    }
    componentWillUnmount() {
        console.log('appContainer', 'componentWillUnmount', this.state)
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
                <div style={{width: '50%', margin: '5px auto', display: 'flex', flexFlow: 'column'}}>
                    {this.state.categories.map(cat => {
                        return (
                            <div key={cat.id} style={{width: '100%', display: 'flex', flexFlow: 'column'}}>
                            <span>{cat.name}</span>
                            <hr />
                                {cat.messages.map(msg => {
                                    return <Message key={msg.id} subject={msg.subject} sender={`${msg.sender.username}@${msg.sender.host}.${msg.sender.domain}`}></Message>
                                })}
                            </div>
                    )
                    })}
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