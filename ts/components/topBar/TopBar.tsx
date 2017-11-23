import * as React from 'react';
import Icons from '../icons/Icons';
import TextBox from '../textBox/TextBox';

import {IconPosition} from '../../enums';

import Logger from '../../logger';
interface ITopBarProps {
    bannerText: string;
    //dispatch
    searchTextUpdate: (value) => void;
}

interface ITopBarState {
    searchText: string;
}

export default class TopBar extends React.Component<ITopBarProps, ITopBarState> {
    constructor(props) {
        super(props);
        this.state = {
            searchText: ''
        };
    }
    render() {
        return (
            <div className="top-bar paper">
                <span>
                    {this.props.bannerText}
                </span>
                <div className="search">
                    <TextBox
                        onChange={(value) => this.updateSearchText(value)}
                        icon={Icons.SearchIcon}
                        iconPosition={IconPosition.Right}
                    />
                </div>
            </div>
        )
    }

    updateSearchText(value: string) {
        this.setState((prev, props) => {
            return {
                searchText: value
            };
        }, () => {
            this.props.searchTextUpdate(value);
        });
    }
}