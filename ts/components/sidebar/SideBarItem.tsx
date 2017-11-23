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
}
export default class SideBarItem extends React.Component<ISideBarItemProps, ISideBarItemState> {
    render() {
        return (
            <div className="side-bar-item">
                <span>{this.props.content}</span>
                {this.props.icon || Icons.letters(this.props.content.slice(0,2), 'sidebar-icon')}
            </div>
        )
    }
}