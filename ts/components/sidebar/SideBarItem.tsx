import * as React from 'react';
import Icons from '../icons/Icons';
import {SideBarState} from '../../enums';
interface ISideBarItemState {

}

interface ISideBarItemProps {
    content: string;
    href: string;
    icon?: JSX.Element;
    width: SideBarState;
    //dispatch
    itemClicked?: (href: string) => void;
}
export default class SideBarItem extends React.Component<ISideBarItemProps, ISideBarItemState> {

    click = ev => {
        let stateObj = { component: this.props.content };
        console.log('pushing state', stateObj, this.props.content, this.props.href);
        window.history.pushState(stateObj, this.props.content, this.props.href);
    }

    render() {
        return (
            <div 
                className="side-bar-item"
                onClick={ev => this.props.itemClicked(this.props.href)}
            >
                <span>{this.props.content}</span>
                {this.props.icon || Icons.letters(this.props.content.slice(0,2), 'sidebar-icon')}
            </div>
        )
    }
}