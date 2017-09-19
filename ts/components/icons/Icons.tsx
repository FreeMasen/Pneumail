import * as React from 'react';
import Icon from './Icon';

export default class Icons {
    static get InboxIcon(): JSX.Element {
        let strokeWidth= 2;
        return (
            <Icon className="inbox-icon sidebar-icon" width={25} height={25}>
                <path
                d={`M 5 40
                    l 45 25
                    l -45 25`}
                fill="rgba(21,221,221, 0.75)"
                />
                <path
                    d={`M 95 40
                        l -45 25
                        l 45 25`
                    }
                    fill="rgba(21,221,221, 0.75)"
                />
                <path
                    d={`M 5 40
                        l 45 -30
                        l 45 30`}
                    fill="#fff"
                />
                <path
                    d={`M 5 90
                        L 95 90`}
                    stroke="rgba(21,221,221, 0.75)"
                    strokeWidth={2}
                />
            </Icon>
        );
    }

    static get SentIcon(): JSX.Element {
        let fill = '#fff'
        return (
            <Icon className="sent-icon" height={25} width={25}>
                <rect
                    x="0"
                    y="75"
                    width="75"
                    height="25"
                    fill={fill}
                />
                <path
                    d={`M 75 75
                        l 0 -15
                        l -8 0`}
                    fill="transparent"
                    stroke={fill}
                    strokeWidth={2}
                    />
                <path
                    d={`M 50 90
                        l 0 -50
                        l -20 0
                        l 0 -20
                        l 20 0
                        l 0 75`}
                    fill="#e82d43"
                    stroke="#e82d43"
                    strokeWidth="10"
                    />
            </Icon>
        )
    }

    static letters(text: string, className: string): JSX.Element {
        return (
            <Icon className={`letter-icon ${className}`} height={25} width={25}>
                <text x="50" y="75" textAnchor="middle">
                {text.toUpperCase()}
                </text>
            </Icon>
        );
    }

}
