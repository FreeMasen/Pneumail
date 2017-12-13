import * as React from 'react';
import SideBar from './components/sidebar/SideBar';
import TopBar from './components/topBar/TopBar';
import {SideBarOption} from './models/option'
import Icons from './components/icons/Icons';
import SearchBar from './components/searchBar/SearchBar';
import {SideBarState, UpdateType} from './enums';
import DataService from './services/dataService';
import Messages from './components/messages/Messages';
import Settings from './components/settings/Settings';

interface IAppState {
    searchValue: string;
    sideBarState: SideBarState;
    categories: any[];
    path?: string;
    component: JSX.Element;
    rules: any[];
    services: any[];
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
            path: 'messages/incompolete',
            component: null,
            rules: [],
            services: [],
        }
        this.db.listen((event: IUpdate) => this.updateState(event));
        window.addEventListener('popstate', ev => this.setState((prev, props) => {
            return {
                path: ev.state.href,
            };
        }));
    }

    updateState = (update: IUpdate) => {
        switch (update.event) {
            case 'categories':
                this.setState((prev, props) => {
                    return {
                        categories: update.data,
                    };
                });
            break;
            case 'rules':
                this.setState((prev, props) => {
                    return {
                        rules: update.data,
                    };
                });
            break;
            case 'services':
                this.setState((prev, props) => {
                    return {
                        services: update.data,
                    };
                });
            break;
        }
    }

    navigationClicked(href: string) {
        if (location.href != href) {
            history.pushState({href: href}, '', href);
        }
        if (href != this.state.path) {
            this.setState((prev, props) => {
                return {
                    path: href,
                    component: this.switchMainContent(href),
                }
            });
        }
    }

    switchMainContent(href: string): JSX.Element {
        let component = this.state.component;
        if (/\/messages\/[a-zA-Z0-9]+/.test(href)) {
            let split = href.split('/');
            return this.switchMessages(split[split.length - 1]);
        }
        if (href == '/settings') {
            return <Settings
                            Rules={this.state.rules}
                            EmailServices={this.state.services}
                            updateService={service => this.updateService(service)}
                            updateRule={rule => this.updateRule(rule)}
                        />
        }
    }

    switchMessages(categoryName: string): JSX.Element {
        let index = this.state.categories.findIndex(c => c.name == categoryName)
        if (index < 0) return;
        let category = this.state.categories[index] as ICategory;
        return (
            <Messages
                messages={category.messages}
                expanded={this.state.sideBarState == SideBarState.Closed}
                title={category.name}
            />
        )
    }

    componentWillMount() {
        if (location.pathname == '') {
            history.replaceState({href: 'messages/incomplete'}, 'Pneumail - incomplete', 'messages/incomplete');
            this.setState((prev, props) => {
                return {
                    path: 'messages/incomplete',
                    component: this.switchMainContent('messages/incomplete')
                }
            })
        } else {
            this.setState((prev, props) => {
                return {
                    path: location.pathname,
                    component: this.switchMainContent(location.pathname)
                }
            });
        }
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
                        options={
                            this.state.categories.map(c => {
                                return new SideBarOption(c.name, `/messages/${c.name}`, Icons.letters(c.name.substring(0,2), `${c.name}-sidebar`))
                            }).concat([new SideBarOption('Settings', '/settings', Icons.letters('se', 'settings-sidebar'))])}
                        width={this.state.sideBarState}
                        toggleWidth={() => this.toggleSidebar()}
                        elementClicked={href => this.navigationClicked(href)}
                    />
                    <div className={`main-container${this.state.sideBarState == SideBarState.Closed ? ' expanded' : ''}`}>
                        {this.switchMainContent(this.state.path)}
                    </div>
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

    updateService = (service: IEmailService) => {
        this.db.sendServiceUpdate(service);
    }

    updateRule = (rule: IRule) => {
        this.db.sendRuleUpdate(rule);
    }
}