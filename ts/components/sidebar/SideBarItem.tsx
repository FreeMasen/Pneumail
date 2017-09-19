import * as React from 'react';
import {Link} from 'react-router-dom';
import Icons from '../icons/Icons';
interface ISideBarItemState {

}

interface ISideBarItemProps {
    content: string;
    href: string;
    icon?: JSX.Element;
    //dispatch
}
export default class SideBarItem extends React.Component<ISideBarItemProps, ISideBarItemState> {
    render() {
        return (
            <Link className="side-bar-item"
                to={this.props.href}
            >
                <span>{this.props.content}</span>
                {this.props.icon || Icons.letters(this.props.content.slice(0,2), 'sidebar-icon')}
            </Link>
        )
    }
}