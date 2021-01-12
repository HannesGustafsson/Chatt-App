import styled from "styled-components";
import Message from "./Message"

const Log = () => {
    return (
            <StyleOuter>
                <Style>
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                    <Message />
                </Style>
            </StyleOuter>
    )
}

const StyleOuter = styled.div`
width: 400px;
height: 650px;
padding: 7px;
border: 2px solid #07333a;
border-radius: 20px;
border-style: solid;
top: 50%;
left: 50%;
transform: translate(-50%, -0%);
position: relative;
background: #f2ffe5;
`

const Style = styled.div`
width: 300px;
height: 550px;
padding: 50px;

overflow-y: scroll;

::-webkit-scrollbar {
    // Width of vertical scroll bar
    width: 8px;
    // Height of horizontal scroll bar
    height: 5px;
  }
  ::-webkit-scrollbar-thumb {
    border-radius: 8px;
    background: #c2c9d2;
  }
  ::-webkit-scrollbar-track {
    -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3); 
    border-radius: 10px;
}
`

export default Log