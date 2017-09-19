import * as React from 'react';

interface ISearchBarState {
    searchTerm: string;
}

interface ISearchBarProps {

    //dispatch
    onChange?: (text: string) => void;
}

export default class SearchBar extends React.Component<ISearchBarProps, ISearchBarState> {
    constructor(props) {
        super(props);
        this.state = {
            searchTerm: ''
        };
    }
    render(): JSX.Element {
        return (
            <div className="search-bar">
                <input
                    type="text"
                    onChange={this.change}
                    onBlur={this.blur}
                />

            </div>
        );
    }
    change(event) {
        let value = (event.currentTarget as HTMLInputElement).value;
        this.setState((prev, props) => {
            return {
                searchTerm: value
            }
        })
    }

    blur(event) {
        let value = (event.currentTarget as HTMLInputElement).value;
        if (this.props.onChange) {
            this.props.onChange(value)
        }
    }
}