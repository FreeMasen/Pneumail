import * as React from 'react';
import TextBox from '../textBox/TextBox';
import { SearchLocation } from '../../enums';

interface IRuleState {
    error: boolean;
    searchTerm: string;
    location: number;
}

interface IRuleProps {
    rule: IRule;
    onSave: (rule: IRule) => void
}

export default class Rule extends React.Component<IRuleProps, IRuleState> {

    constructor(props) {
        super(props);
        this.state = {
            error: false,
            location: props.rule.location || 0,
            searchTerm: props.rule.searchTerm || ''
        }
    }
    render() {
        console.log(this.props.rule.location);
        return (
            <div className={`rule-form${this.state.error ? ' error' : ''}`}>
                <div className="input-group">
                    <label htmlFor={`${this.props.rule.id}-address")`} className="input-label">Address</label>
                    <TextBox
                        initialValue={this.state.searchTerm}
                        onChange={value => this.updateSearchTerm(value)}
                        accentColor="rgb(231,209,43)"
                        textColor="#000"

                    />
                </div>
                <div className="location-container">
                    <label>Location</label>
                    <div>
                        <div className="checkbox-group">
                            <label>Subject</label>
                            <input
                                type="checkbox"
                                checked={(this.state.location & SearchLocation.Subject) > 0}
                                onChange={ev => this.updateLocation(SearchLocation.Subject, ev.currentTarget.checked)}
                            />
                        </div>
                        <div className="checkbox-group">
                            <label>Body</label>
                            <input
                                type="checkbox"
                                checked={(this.state.location & SearchLocation.Body) > 0}
                                onChange={ev => this.updateLocation(SearchLocation.Body, ev.currentTarget.checked)}
                            />
                        </div>
                        <div className="checkbox-group">
                            <label>From</label>
                            <input
                                type="checkbox"
                                checked={(this.state.location & SearchLocation.From) > 0}
                                onChange={ev => this.updateLocation(SearchLocation.From, ev.currentTarget.checked)}

                            />
                        </div>
                        <div className="checkbox-group">
                            <label>To</label>
                            <input
                                type="checkbox"
                                checked={(this.state.location & SearchLocation.To) > 0}
                                onChange={ev => this.updateLocation(SearchLocation.To, ev.currentTarget.checked)}
                            />
                        </div>
                    </div>
                </div>
                <button type="button" onClick={ev => this.save()} className="form-button submit">Save</button>
            </div>
        );
    }

    updateLocation(value: number, add: boolean) {
        console.log('updateLocation', value, add);
        this.setState((prev, props) => {
            if (add) {
                return {
                    location: prev.location + value
                }
            } else {
                return {
                    location: prev.location - value,
                }
            }
        })

    }

    updateSearchTerm(value: string) {
        this.setState((prev, props) => {
            return {
                searchTerm: value
            }
        })
    }

    save() {
        if (this.state.location > 0
            && this.state.searchTerm) {
            this.props.onSave({
                searchTerm: this.state.searchTerm,
                location: this.state.location
            });
        } else {
            this.setState((prev, props) => {
                return {
                    error: true
                }
            })
        }
     }
}