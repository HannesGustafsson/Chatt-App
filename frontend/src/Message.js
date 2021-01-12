
import styled from "styled-components";

const Message = () => {
  return (
    <StyleMessage>
      <StyleUser>
          User User 00:00
      </StyleUser>
      Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
    </StyleMessage>
    )
  }

  const StyleMessage = styled.div`
font-family: Helvetica, sans-serif;
font-size: small;
color: #454545;
`
const StyleUser = styled.div`
font-weight: bold;
font-size: medium;
`

  export default Message