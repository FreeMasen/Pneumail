import * as React from 'react';
import ExpandToggle from '../buttons/expandToggle';
import SideBarItem from './SideBarItem';
import {SideBarOption} from '../../models/option'
import {SideBarState} from '../../enums';

interface ISideBarState {
}

interface ISideBarProps {
    width: SideBarState;
    title: string;
    toggle?: JSX.Element;
    options: Array<SideBarOption>;
    //Dispatch
    toggleWidth: () => void;
    elementClicked?: (href: string) => void;
}

export default class SideBar extends React.Component<ISideBarProps, ISideBarState> {

    private defaultToggle = <ExpandToggle
                                onClick={() => this.props.toggleWidth()}
                            />
    render() {

        return (
            <div className="side-bar-container">
                <div className={`side-bar paper ${this.props.width == SideBarState.Open ? 'open' : 'closed'}`}>
                    <div className="side-bar-title-container">
                        <span className="side-bar-title">
                            {this.props.title}
                        </span>
                        <div className="toggle-container">
                            {this.props.toggle || this.defaultToggle}
                        </div>
                    </div>
                    {this.props.options.map((option, i) =>
                        <SideBarItem
                            key={`side-bar-item-${i}`}
                            content={option.text}
                            href={option.value}
                            icon={option.icon}
                            width={this.props.width}
                            itemClicked={href => this.props.elementClicked(href)}
                        />)}
                </div>
            </div>
        );
    }

    toggle() {
        this.props.toggleWidth()
    }
}