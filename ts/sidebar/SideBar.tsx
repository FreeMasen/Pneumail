import * as React from 'react';
import ExpandToggle from '../buttons/expandToggle';

interface ISideBarState {
    expanded: boolean;
}

interface ISideBarProps {
    expanded: boolean;
    title: string;
    toggle?: JSX.Element;
    //Dispatch
}

export default class SideBar extends React.Component<ISideBarProps, ISideBarState> {
    private defaultToggle = <ExpandToggle
                                onClick={this.toggle}
                            />
    constructor(props: ISideBarProps) {
        super(props);
        this.state = {
            expanded: props.expanded
        };
    }
    render() {
        return (
            <div className="side-bar">
                <div className="side-bar-title-container">
                    <span className="side-bar-title">
                        {this.props.title}
                    </span>
                    <div className="toggle-container">
                        {this.props.toggle || this.defaultToggle}
                    </div>
                </div>
            </div>
        );
    }

    toggle() {

    }
}